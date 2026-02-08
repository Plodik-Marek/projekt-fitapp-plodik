using fitapp_plodik_MVC.Entities;
using fitapp_plodik_MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace fitapp_plodik_MVC.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<fitapp_plodik_MVC.Entities.Machine> Machines { get; set; } //nešlo mi napsat jen machine jako entitu tkaže jsem si poradil tímto způsobem
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<TrainerSpecialization> TrainerSpecializations { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=4c2_plodikmarek_db2;user=plodikmarek;password=123456");
        }
    }
}
