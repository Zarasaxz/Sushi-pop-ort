using _20241CYA12B_G2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Data;
using System.Diagnostics;

namespace _20241CYA12B_G2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbContext _context;

        public HomeController(DbContext context)
        {
            _context = context;
        }

        public async  Task <IActionResult> Index()
        {

            var nroDia = (int)DateTime.Today.DayOfWeek;

            var nombreDia= System.Globalization.CultureInfo.GetCultureInfo("es-ES").DateTimeFormat.GetDayName
                ((DayOfWeek)nroDia);

            var descuento = await _context.Descuento.Include(d=>d.Producto).FirstOrDefaultAsync(d => d.Dia == nroDia && d.Activo);

           


            HomeViewModel homeViewModel = new HomeViewModel();

            homeViewModel.nombreDia = nombreDia;


            if (descuento == null)
            {
                homeViewModel.mensajeDescuento = "Hoy es " + nombreDia + " disfrutá del mejor sushi en casa con amigos";

            }
            else
            {
                homeViewModel.descuento = descuento.Porcentaje + "% ";
                homeViewModel.Producto = descuento.Producto.Nombre;
            }

            if (nroDia > 0 && nroDia <= 4)
            {
                homeViewModel.mensajeAtencion = " atendemos de 19 a 23 horas por WhatsApp";
              
            }
            else 
            {
                homeViewModel.mensajeAtencion = " atendemos de 11 a 14 horas y de 19 a 23 horas por WhatsApp";

            }


            return View(homeViewModel);

        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
