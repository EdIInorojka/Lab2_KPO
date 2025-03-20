using Lab2_KPO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_KPO.Data
{
    public class AppDbContext : DbContext
    {

        public static event Action DataChanged;
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=UniversityDB;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Faculty)
                .WithMany(f => f.Groups)
                .HasForeignKey(g => g.FacultyId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId);
        }

        public static void NotifyDataChanged()
        {
            DataChanged?.Invoke();
        }
    }
}
