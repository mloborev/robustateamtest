using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskRobusta.Models;

namespace TestTaskRobusta.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<TotalForMonth> TotalForMonths { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TestTaskDB;Trusted_Connection=True;");
        }
    }
}
