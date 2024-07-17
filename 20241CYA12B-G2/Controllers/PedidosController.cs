using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _20241CYA12B_G2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using SQLitePCL;

namespace _20241CYA12B_G2.Controllers
{
    public class PedidosController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public PedidosController(DbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Pedidos
        [Authorize(Roles ="CLIENTE,EMPLEADO")]
        public async Task<IActionResult> Index()
        {
            var listaPedidos = await _context.Pedido
                .Include(p => p.Carrito)
                .Include(p => p.Carrito.Cliente)
                .ToListAsync();


            //cree una variable listaPedidosTask y luego la guarde dentro de listaPedidos porque sino el where daba error

            //averiguar si es cliente o empleado

            if (User.IsInRole("CLIENTE"))
            {
                var user = await _userManager.GetUserAsync(User);

                var pedidosCliente = listaPedidos.Where(p => p.Carrito.Cliente.Email.ToUpper() == user.Email.ToUpper()
                && p.FechaCompra >= DateTime.Now.AddDays(-90)).ToList();

                return View(pedidosCliente);

            }

            var pedidosEmpleado = listaPedidos.Where(p => p.Estado != 5 && p.Estado != 6).ToList();

            return View(pedidosEmpleado);

             }

        [Authorize(Roles = "CLIENTE")]
        public async Task<IActionResult> HacerPedido(int idCarrito)
        {
            var carrito = await _context.Carrito.Include(c => c.Cliente)
                                                .Include(c => c.CarritoItems)
                                                .ThenInclude(ci => ci.Producto)
                                                .FirstOrDefaultAsync(c => c.Id == idCarrito);


            decimal subtotal = carrito.CarritoItems.Sum(ci => ci.PrecioUnitarioConDescuento * ci.Cantidad);
            decimal gastoEnvio = 0;
            decimal clima = 22.05m;

            if(cantidadPedidos().Result < 10)
            {
                //VERIFICAR CLIMA
                if(clima < 5)
                {
                    gastoEnvio *= 1.5m;
                }
                else
                {
                    gastoEnvio = 80;
                }
            }

            DetallePedidoViewModel model = new DetallePedidoViewModel
            {
                identificadorCarrito = idCarrito,
                Cliente = carrito.Cliente.Nombre + " " + carrito.Cliente.Apellido,
                Direccion = carrito.Cliente.Direccion,
                SubTotal = subtotal,
                GastoEnvio = gastoEnvio,
                Total = subtotal + gastoEnvio,
                Productos = carrito.CarritoItems.Select(ci => ci.Producto.Nombre).ToList()
            };

            return View("DetallePedido", model);
        }

        public async Task<int> cantidadPedidos()
        {
            var user = await _userManager.GetUserAsync(User);
            Cliente cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.Email == user.Email);

            int pedidosConfirmados = 0;
            var pedidos = await _context.Carrito
                .Include(c => c.Pedido)
                .Where(p => p.ClienteId == cliente.Id).ToListAsync();

            
                foreach (var pedido in pedidos)
                {
                    if (pedido.Pedido != null)
                    {
                    if (pedido.Pedido.Estado == 2)
                        pedidosConfirmados++;
                    }
                }
            
            return pedidosConfirmados;
            
        }

        public async Task<IActionResult> ConfirmarPedido(int a)
        {
            var carrito = await _context.Carrito.Include(c => c.Cliente)
                                                .Include(c => c.CarritoItems)
                                                .ThenInclude(ci => ci.Producto)
                                                .FirstOrDefaultAsync(c => c.Id == a);

            carrito.Procesado = true;
            _context.Update(carrito);
            await _context.SaveChangesAsync();

            decimal subtotal = carrito.CarritoItems.Sum(ci => ci.PrecioUnitarioConDescuento * ci.Cantidad);
            decimal gastoEnvio = 0;
            decimal clima = 22.05m;

            if (cantidadPedidos().Result < 10)
            {
                //VERIFICAR CLIMA
                if (clima < 5)
                {
                    gastoEnvio *= 1.5m;
                }
                else
                {
                    gastoEnvio = 80;
                }
            }
                  

            if (carrito != null)
            {
                Pedido pedido = new Pedido
                {
                    Estado = 1,
                    Subtotal = subtotal,
                    CarritoId = a,
                    GastoEnvio = gastoEnvio,
                    Total = subtotal + gastoEnvio,
                    FechaCompra = DateTime.Now,
                    Carrito = _context.Carrito.Where(c => c.Id == a).FirstOrDefaultAsync().Result,
                    NroPedido = await GenerarNumeroPedido()
                };
                _context.Add(pedido);
                await _context.SaveChangesAsync();


            }


            return RedirectToAction(nameof(Index));
        }


        // GET: Pedidos/Details/5
        [Authorize (Roles = "CLIENTE")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Carrito)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [Authorize(Roles = "CLIENTE")]
        public IActionResult Create()
        {
            // Obtener los carritos que no tienen pedidos asociados
            var carritosSinPedidos = _context.Carrito
                                              .Where(c => !_context.Pedido.Any(p => p.CarritoId == c.Id))
                                              .Select(c => new { c.Id })
                                              .ToList();

            ViewBag.CarritoId = new SelectList(carritosSinPedidos, "Id", "Id");
            return View();
        }


        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> 
            Create([Bind("Id,NroPedido,FechaCompra,Subtotal,GastoEnvio,Total,Estado,CarritoId")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "Id", "Id", pedido.CarritoId);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        [Authorize (Roles = "EMPLEADO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "Id", "Id", pedido.CarritoId);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NroPedido,FechaCompra,Subtotal,GastoEnvio,Total,Estado,CarritoId")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "Id", "Id", pedido.CarritoId);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Carrito)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pedido == null)
            {
                return Problem("Entity set 'DbContext.Pedido'  is null.");
            }
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedido.Remove(pedido);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
          return (_context.Pedido?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private async Task<int> GenerarNumeroPedido()
        {
            var pedido= await _context.Pedido.OrderByDescending(c => c.NroPedido).FirstOrDefaultAsync();

            if (pedido == null)
            {
                return 30000;
            }

            return (int)pedido.NroPedido + 5;
        }
    }
}
