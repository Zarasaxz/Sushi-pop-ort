using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _20241CYA12B_G2.Models;

namespace _20241CYA12B_G2.Controllers
{
    public class ReclamosController : Controller
    {
        private readonly DbContext _context;

        public ReclamosController(DbContext context)
        {
            _context = context;
        }

        // GET: Reclamos
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.Reclamo.Include(r => r.Pedido);
            return View(await dbContext.ToListAsync());
        }

        // GET: Reclamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reclamo == null)
            {
                return NotFound();
            }

            var reclamo = await _context.Reclamo
                .Include(r => r.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reclamo == null)
            {
                return NotFound();
            }

            return View(reclamo);
        }

        // GET: Reclamos/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "Id");
            return View();
        }

        // POST: Reclamos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreCompleto,Email,Telefono,DetalleReclamo,PedidoId")] Reclamo reclamo)
        {
            reclamo.Id = await GenerarNumeroReclamo();
            if (ModelState.IsValid)
            {
                _context.Add(reclamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "Id", reclamo.PedidoId);
            return View(reclamo);
        }

        // GET: Reclamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reclamo == null)
            {
                return NotFound();
            }

            var reclamo = await _context.Reclamo.FindAsync(id);
            if (reclamo == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "Id", reclamo.PedidoId);
            return View(reclamo);
        }

        // POST: Reclamos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,Email,Telefono,DetalleReclamo,PedidoId")] Reclamo reclamo)
        {
            if (id != reclamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reclamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReclamoExists(reclamo.Id))
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
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "Id", reclamo.PedidoId);
            return View(reclamo);
        }

        // GET: Reclamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reclamo == null)
            {
                return NotFound();
            }

            var reclamo = await _context.Reclamo
                .Include(r => r.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reclamo == null)
            {
                return NotFound();
            }

            return View(reclamo);
        }

        // POST: Reclamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reclamo == null)
            {
                return Problem("Entity set 'DbContext.Reclamo'  is null.");
            }
            var reclamo = await _context.Reclamo.FindAsync(id);
            if (reclamo != null)
            {
                _context.Reclamo.Remove(reclamo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReclamoExists(int id)
        {
          return (_context.Reclamo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private async Task<int> GenerarNumeroReclamo()
        {
            var reclamo = await _context.Reclamo.OrderByDescending(c => c.Id).FirstOrDefaultAsync();

            if (reclamo == null)
            {
                return 0;
            }

            return (int)reclamo.Id + 1;
        }
    }
}
