using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitapp_plodik_MVC.Entities
{
    [Table("workouts")]
    public class Workout
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("workout_date")]
        public DateTime WorkoutDate { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }

}

