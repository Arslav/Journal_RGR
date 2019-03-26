using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journal_RGR.Models
{
    public class ApplicationContext : DbContext
    {
        private readonly string _dbPath;

        public DbSet<Student> Students { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Journal> Journals { get; set; }

        public ApplicationContext()
        {
            _dbPath = "journal.db";
        }

        public ApplicationContext(string path)
        {
            _dbPath = path;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_dbPath}");
        }
    }
}
