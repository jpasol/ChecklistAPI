using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChecklistAPI
{
    public class IssuesContext:DbContext
    {
        public IssuesContext(DbContextOptions<IssuesContext>options):base(options)
        {
                
        }
        public DbSet<Issues> Issues { get; set; }
    }
}
