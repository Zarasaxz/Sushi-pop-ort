using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _20241CYA12B_G2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class DbContext : IdentityDbContext
    {
        public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<_20241CYA12B_G2.Models.Contacto> Contacto { get; set; } = default!;

        public DbSet<_20241CYA12B_G2.Models.Carrito>? Carrito { get; set; }

        public DbSet<_20241CYA12B_G2.Models.CarritoItem>? CarritoItem { get; set; }

        public DbSet<_20241CYA12B_G2.Models.Categoria>? Categoria { get; set; }

        public DbSet<_20241CYA12B_G2.Models.Cliente>? Cliente { get; set; }

        public DbSet<_20241CYA12B_G2.Models.Descuento>? Descuento { get; set; }

        public DbSet<_20241CYA12B_G2.Models.Empleado>? Empleado { get; set; }

        public DbSet<_20241CYA12B_G2.Models.Pedido>? Pedido { get; set; }

        public DbSet<_20241CYA12B_G2.Models.Producto>? Producto { get; set; }

        public DbSet<_20241CYA12B_G2.Models.Reclamo>? Reclamo { get; set; }

        public DbSet<_20241CYA12B_G2.Models.Reserva>? Reserva { get; set; }
    }
