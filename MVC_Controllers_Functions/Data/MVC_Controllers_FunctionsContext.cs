using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_Controllers_Functions.Models;

namespace MVC_Controllers_Functions.Data
{
    public class MVC_Controllers_FunctionsContext : DbContext
    {
        public MVC_Controllers_FunctionsContext (DbContextOptions<MVC_Controllers_FunctionsContext> options)
            : base(options)
        {
        }

        public DbSet<MVC_Controllers_Functions.Models.User> User { get; set; } = default!;
    }
}
