using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace fitapp_plodik_MVC.Entities
{
    [Table("machines")]
    public class Machine
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("brand")]
        public string? Brand { get; set; }

        [Required]
        [Column("muscle_group")]
        public string MuscleGroup { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

    }
}
