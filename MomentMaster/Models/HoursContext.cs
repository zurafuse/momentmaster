using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MomentMaster.Models
{
    public class HoursContext : DbContext
    {
        public HoursContext(DbContextOptions<HoursContext> options) : base(options)
        {

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            System.Diagnostics.Debug.WriteLine("A Time Entry has been changed or modified in the database.");
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Hours> Hours { get; set; }
        public DbSet<TimeObject> TimeObject { get; set; }

        public DbSet<User> User { get; set; }

    }
}
