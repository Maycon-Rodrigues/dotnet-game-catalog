using System.ComponentModel.DataAnnotations;

namespace GameCatalog.InputModels
{
    public class GameInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The game name must contain between 3 and 100 characters.")]
        public string Title { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The producer's name must contain between 3 and 100 characters.")]
        public string Producer { get; set; }
        
        [Required]
        // [MinLength(1, ErrorMessage = "The price cannot be zero.")]
        public double Price { get; set; }
    }
}