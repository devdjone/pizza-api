using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pizza_api.Models;

namespace pizza_api.Data
{
    public class pizza_apiContext : DbContext
    {
        public pizza_apiContext (DbContextOptions<pizza_apiContext> options)
            : base(options)
        {
        }

        public DbSet<pizza_api.Models.Pizza> Pizza { get; set; } = default!;
    }
}
