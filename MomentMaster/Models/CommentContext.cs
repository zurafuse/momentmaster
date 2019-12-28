using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MomentMaster.Models
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            System.Diagnostics.Debug.WriteLine("A Comment has been changed or modified in the database.");
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<TimeObject> TimeObject { get; set; }

    }
}
