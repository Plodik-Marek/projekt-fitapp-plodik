using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitapp_plodik_MVC.Entities
{
    [Table("trainers")]
    public class Trainer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("full_name")]
        public string FullName { get; set; } = null!;

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        public ICollection<TrainerSpecialization> TrainerSpecializations { get; set; } = new List<TrainerSpecialization>();

    }
}
