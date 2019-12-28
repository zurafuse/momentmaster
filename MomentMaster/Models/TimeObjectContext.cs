using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MomentMaster.Models
{
    public class TimeObjectContext : DbContext
    {
        public TimeObjectContext(DbContextOptions<TimeObjectContext> options) : base(options)
        {

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            System.Diagnostics.Debug.WriteLine("A Project has been changed or modified in the database.");
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<TimeObject> TimeObject { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Hours> Hours { get; set; }

    }
}
