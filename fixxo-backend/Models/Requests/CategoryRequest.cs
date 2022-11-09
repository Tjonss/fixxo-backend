using System.ComponentModel.DataAnnotations;

namespace fixxo_backend.Models.Requests
{
    public class CategoryRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
