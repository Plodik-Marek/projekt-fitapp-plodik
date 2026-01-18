using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace fitapp_plodik_MVC.Entities
{
    [Table("workout_exercises")]
    public class WorkoutExercise
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("workout_id")]
        public int WorkoutId { get; set; }

        [ValidateNever]
        public Workout Workout { get; set; } = null!;

        [Required]
        [Column("exercise_id")]
        public int ExerciseId { get; set; }

        [ValidateNever]
        public Exercise Exercise { get; set; } = null!;

        [Required]
        [Column("sets")]
        public int Sets { get; set; }

        [Required]
        [Column("reps")]
        public int Reps { get; set; }

        [Column("weight")]
        public decimal? Weight { get; set; }

        [Column("note")]
        public string? Note { get; set; }

    }
}
