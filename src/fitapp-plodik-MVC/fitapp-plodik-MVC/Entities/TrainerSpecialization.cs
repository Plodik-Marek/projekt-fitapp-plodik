using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitapp_plodik_MVC.Entities
{
    [Table("trainer_specializations")]
    public class TrainerSpecialization
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("trainer_id")]
        public int TrainerId { get; set; }

        public Trainer Trainer { get; set; } = null!;

        [Required]
        [Column("exercise_id")]
        public int ExerciseId { get; set; }

        public Exercise Exercise { get; set; } = null!;

    }
}
