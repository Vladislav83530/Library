using System.ComponentModel.DataAnnotations;

namespace Library.BLL.DTOs
{
    public class RatingRequestDTO
    {
        [Range(1, 5)]
        public int Score { get; set; } 
    }
}
