using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fitapp_plodik_MVC.Entities;

namespace fitapp_plodik_MVC.Entities

{
    [Table("exercises")]
    public class Exercise
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("muscle_group")]
        public string? MuscleGroup { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("machine_id")]
        public int? MachineId { get; set; }  //může být null tak ?

        public Machine? Machine { get; set; }

        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

        public ICollection<TrainerSpecialization> TrainerSpecializations { get; set; } = new List<TrainerSpecialization>();
    }
}
